import {
    SHOW_PROGRESS,
    HIDE_PROGRESS

} from "../actions/progressbar";

const state = {
    isShow: false,
    processList: []
};


const getters = {
    isShowProgressBar: state => !!state.isShow,
    processList: state => state.processList
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
        // var isExits = (state.processList.map(e => e.Id == processInfo.Id).indexOf(true) > -1);
        var newList = state.processList.filter(e => e.Id != processInfo.Id);
        newList.push(processInfo);
        state.processList = newList;
    },
    [HIDE_PROGRESS]: (state) => {
        state.isShow = false;
        state.processList = []
    },
};

export default {
    state,
    getters,
    actions,
    mutations
};