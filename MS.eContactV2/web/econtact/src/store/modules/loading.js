import {
    SHOW_LOADING,
    HIDE_LOADING

} from "../actions/loading";

const state = {
    isShow: false
};


const getters = {
    isShowLoading: state => !!state.isShow
};

const actions = {
    [SHOW_LOADING]: ({ commit }) => {
        commit("SHOW_LOADING");
    },
    [HIDE_LOADING]: ({ commit }) => {
        commit("HIDE_LOADING");
    },
};

const mutations = {
    [SHOW_LOADING]: (state) => {
        state.isShow = true;
    },
    [HIDE_LOADING]: (state) => {
        state.isShow = false;
    },
};

export default {
    state,
    getters,
    actions,
    mutations
};