import {
    SET_NEW_NOTICE_NUMBER,
    CLEAR_NEW_NOTICE_NUMBER
} from "../actions/signalR";

const state = {
    // isError: true,
    newNotifications: 0
};


const getters = {
    newNotifications: state => !!(state.newNotifications),
};

const actions = {
    [SET_NEW_NOTICE_NUMBER]: ({ commit }, { number }) => {
        commit("SET_NEW_NOTICE_NUMBER", { number });
    },
    [CLEAR_NEW_NOTICE_NUMBER]: ({ commit }) => {
        commit("CLEAR_NEW_NOTICE_NUMBER");
    },
};

const mutations = {
    [SET_NEW_NOTICE_NUMBER]: async(state, { number }) => {
        state.newNotifications = number;
    },
    [CLEAR_NEW_NOTICE_NUMBER]: (state) => {
        state.newNotifications = 0;
    },
};

export default {
    state,
    getters,
    actions,
    mutations
};