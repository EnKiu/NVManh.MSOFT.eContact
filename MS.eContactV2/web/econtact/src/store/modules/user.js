import { USER_REQUEST, USER_ERROR, USER_SUCCESS } from "../actions/user";
import apiCall from "../../utils/api";
// import { createApp } from 'vue'
import { AUTH_LOGOUT } from "../actions/auth";
// const app = createApp({});

const state = {
    status: "",
    role: null,
    isReload: true,
    profile: {}
};

const getters = {
    getProfile: state => state['profile'],
    isProfileLoaded: state => !!state.profile.name,
    role: state => {
        if (state.isReload)
            return localStorage.getItem("userRole");
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
            apiCall({ url: `/api/v1/users/${userId}`, method: "GET" })
                .then(resp => {
                    commit(USER_SUCCESS, resp);
                    localStorage.setItem("userName", resp.UserName);
                    localStorage.setItem("avatar", resp.Employee.AvatarFullPath);
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
        localStorage.setItem("userRole", resp.RoleValue);
        // app.$set(state, "profile", resp);

    },
    [USER_ERROR]: state => {
        state.status = "error";
    },
    [AUTH_LOGOUT]: state => {
        state.profile = {};
        state.role = null;
        state.isReload = true;
    }
};

export default {
    state,
    getters,
    actions,
    mutations
};