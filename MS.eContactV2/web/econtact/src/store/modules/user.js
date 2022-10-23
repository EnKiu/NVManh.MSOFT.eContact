import { USER_REQUEST, USER_ERROR, USER_SUCCESS } from "../actions/user";
import apiCall from "../../utils/api";
// import { createApp } from 'vue'
import { AUTH_LOGOUT } from "../actions/auth";
// const app = createApp({});
import router from "@/router";

const state = {
    status: "",
    role: null,
    isReload: true,
    profile: {}
};

const getters = {
    getProfile: state => state['profile'],
    isProfileLoaded: state => !!state.profile.UserName,
    role: state => {
        if (state.isReload)
            return localStorage.getItem("userRoleValue");
        else
            return state.role;
    }
};

const actions = {
    /**
     * Lấy thông tin của user
     * @param {*} param0 
     */
    [USER_REQUEST]: ({ commit, dispatch }) => {
        return new Promise((resolve, reject) => {
            commit(USER_REQUEST);
            var userId = localStorage.getItem('user-id');
            apiCall({ url: `/api/v1/accounts/${userId}`, method: "GET" })
                .then(resp => {
                    var roles = resp.Roles;
                    if (roles && roles.length > 0) {
                        localStorage.setItem("userRoleValue", roles[0].RoleValue);
                        resp.RoleValue = roles[0].RoleValue;
                    }
                    localStorage.setItem("userName", resp.UserName);
                    localStorage.setItem("avatar", resp.AvatarFullPath);
                    localStorage.setItem("firstName", resp.firstName);
                    localStorage.setItem("lastName", resp.LastName);
                    localStorage.setItem("fullName", resp.FullName);
                    localStorage.setItem("contactId", resp.ContactId);
                    commit(USER_SUCCESS, resp);
                    resolve(resp);
                })
                .catch((res) => {
                    commit(USER_ERROR);
                    // if resp is unauthorized, logout, to
                    dispatch(AUTH_LOGOUT);
                    reject(res);
                });
        })

    }
};

const mutations = {
    [USER_REQUEST]: state => {
        state.status = "loading";
    },
    [USER_SUCCESS]: (state, resp) => {
        state.status = "success";
        state['profile'] = resp;
        state.role = resp.RoleValue;
        state['isReload'] = false;
        // localStorage.setItem("userRole", resp.RoleValue);
        // app.$set(state, "profile", resp);

    },
    [USER_ERROR]: state => {
        state.status = "error";
    },
    [AUTH_LOGOUT]: state => {
        state.profile = {};
        state.role = null;
        state.isReload = true;
        router.push("/login");
    }
};

export default {
    state,
    getters,
    actions,
    mutations
};