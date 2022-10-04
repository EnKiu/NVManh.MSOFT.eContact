import {
    SET_ERROR_MSG,
    CLEAR_ERROR_MSG
} from "../actions/notification";
import MISAEnum from "@/scripts/enum";

const state = {
    // isError: true,
    errors: [],
    title: "Thông báo",
    type: MISAEnum.MsgType.Error,
    confirmFunction: null,
    showCancelButton: false
};


const getters = {
    isShowError: state => !!(state.errors.length > 0),
    errorsNotification: state => state.errors,
    titleNotification: state => state.title,
    msgType: state => state.type,
    confirmFunction: state => state.confirmFunction,
    showCancelButton: state => state.showCancelButton
};

const actions = {
    [SET_ERROR_MSG]: ({ commit }, { title, msg, type, confirm, showCancelButton }) => {
        commit("SET_ERROR_MSG", { title, msg, type, confirm, showCancelButton });
    },
    [CLEAR_ERROR_MSG]: ({ commit }) => {
        commit("CLEAR_ERROR_MSG");
    },
};

const mutations = {
    [SET_ERROR_MSG]: async(state, { title, msg, type, confirm, showCancelButton }) => {
        state.title = title;
        state.type = type;
        state.errors = [];
        state.confirmFunction = confirm;
        if (showCancelButton) {
            state.showCancelButton = showCancelButton;
        }
        if (typeof msg === 'string' || msg instanceof String || typeof msg == 'number') {
            state.errors.push(msg);
        } else if (Array.isArray(msg)) {
            for (const itemChild of msg) {
                if (typeof itemChild === 'string' || itemChild instanceof String || typeof itemChild == 'number') {
                    state.errors.push(itemChild);
                }
            }
        } else {
            state.errors.push("Có lỗi xảy ra khi xử lý thông tin, vui lòng thử lại hoặc liên hệ bộ phận hỗ trợ.");
        }

    },
    [CLEAR_ERROR_MSG]: (state) => {
        state.errors = [];
        state.title = null;
        state.type = MISAEnum.MsgType.Info;
        state.showCancelButton = false;
    },
};

export default {
    state,
    getters,
    actions,
    mutations
};