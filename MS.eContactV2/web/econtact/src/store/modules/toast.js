import {
    SET_TOAST,
    CLEAR_TOAST
} from "../actions/toast";
import MISAEnum from "@/scripts/enum";

const state = {
    // isError: true,
    msg: null,
    type: MISAEnum.MsgType.Error,
};


const getters = {
    isShowToast: state => !!(state.msg),
    msgToast: state => state.msg,
    msgToastType: state => state.type
};

const actions = {
    [SET_TOAST]: ({ commit }, { msg, type }) => {
        commit("SET_TOAST", { msg, type });
    },
    [CLEAR_TOAST]: ({ commit }) => {
        commit("CLEAR_TOAST");
    },
};

const mutations = {
    [SET_TOAST]: async(state, { msg, type }) => {
        state.type = type;
        state.msg = msg;
    },
    [CLEAR_TOAST]: (state) => {
        state.msg = null;
        state.type = MISAEnum.MsgType.Info
    },
};

export default {
    state,
    getters,
    actions,
    mutations
};