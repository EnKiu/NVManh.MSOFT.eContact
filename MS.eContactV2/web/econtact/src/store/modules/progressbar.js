import {
    SHOW_PROGRESS,
    HIDE_PROGRESS

} from "../actions/progressbar";

const state = {
    isShow: false,
    processInfo: { Value: 0, Max: 0, Message: "" }
};


const getters = {
    isShowProgressBar: state => !!state.isShow,
    processInfo: state => state.processInfo
};

const actions = {
    [SHOW_PROGRESS]: ({ commit }, processInfo) => {
        commit("SHOW_PROGRESS", processInfo);
    },
    [HIDE_PROGRESS]: ({ commit }) => {
        commit("HIDE_PROGRESS");
    },
};

const mutations = {
    [SHOW_PROGRESS]: (state, processInfo) => {
        state.isShow = true;
        state.processInfo = processInfo;
    },
    [HIDE_PROGRESS]: (state) => {
        state.isShow = false;
        state.processInfo = { Value: 0, Max: 0 }
    },
};

export default {
    state,
    getters,
    actions,
    mutations
};