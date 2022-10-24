import {
    CREATE_CONNECTION,
    SET_NEW_NOTICE_NUMBER,
    CLEAR_NEW_NOTICE_NUMBER,
    SET_CONNECTING_HUB
} from "../actions/signalR";

import webSocket from '../../http/WebSocket';

const state = {
    hubConnection: null,
    // isError: true,
    newNotifications: 0,
    isConnecting: false
};


const getters = {
    newNotifications: state => !!(state.newNotifications),
    hubConnection: state => state.hubConnection,
    connectingHub: state => state.isConnecting
};

const actions = {
    [CREATE_CONNECTION]: ({ commit }) => {
        commit("CREATE_CONNECTION");
    },
    [SET_NEW_NOTICE_NUMBER]: ({ commit }, { number }) => {
        commit("SET_NEW_NOTICE_NUMBER", { number });
    },
    [CLEAR_NEW_NOTICE_NUMBER]: ({ commit }) => {
        commit("CLEAR_NEW_NOTICE_NUMBER");
    },
    [SET_CONNECTING_HUB]: ({ commit }, isConnecting) => {
        commit("SET_CONNECTING_HUB", isConnecting);
    }
};

const mutations = {
    [CREATE_CONNECTION]: () => {
        state.hubConnection = webSocket.createHub();
        state.hubConnection
            .start()
            .then(() => {
                console.log("Đã kết nối tới Hub...");
            })
            .catch((err) => console.error(err));
    },
    [SET_NEW_NOTICE_NUMBER]: async(state, { number }) => {
        state.newNotifications = number;
    },
    [CLEAR_NEW_NOTICE_NUMBER]: (state) => {
        state.newNotifications = 0;
    },
    [SET_CONNECTING_HUB]: (state, isConnecting) => {
        state.isConnecting = isConnecting;
    },
};

export default {
    state,
    getters,
    actions,
    mutations
};