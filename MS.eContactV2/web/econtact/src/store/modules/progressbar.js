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
        var hasInList = false;
        for (const processItem of state.processList) {
            if (processItem.Id == processInfo.Id) {
                processItem.Value = processInfo.Value;
                processItem.Max = processInfo.Max;
                processItem.RunBackground = processInfo.RunBackground;
                processItem.Message = processInfo.Message;
                hasInList = true;
                break;
            }
        }
        if (!hasInList) {
            state.processList.push(processInfo);
        }
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