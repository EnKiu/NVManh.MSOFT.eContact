import {
    CREATE_CONNECTION,
    SET_NEW_NOTICE_NUMBER,
    CLEAR_NEW_NOTICE_NUMBER,
    SET_CONNECTING_HUB
} from "../actions/signalR";

import webSocket from '../../http/WebSocket';
import commonJs from "@/scripts/common";

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
                commonJs.hideConnectingHub();
            })
            .catch((err) => {
                console.error(err);
                commonJs.hideConnectingHub();
            });

        state.hubConnection.on("ReceiveNotificationWhenDisconnected", (username) => {
            console.log(`${username} đã ngắt kết nối!`);
        })
        state.hubConnection.on("ShowAlertWhenOnline", (username) => {
            console.log(`${username} đã kết nối!`);
        })

        state.hubConnection.on("ShowPecentUpload", (currentFileUpload, totalFileUpload, isFinish, totalTimes, progressInfo) => {
            // Ẩn loading nếu có:
            commonJs.hideLoading();
            progressInfo = {
                Id: progressInfo.id,
                Name: progressInfo.name,
                Value: currentFileUpload,
                Max: totalFileUpload,
                Message: `Đang tải lên ${currentFileUpload}/${totalFileUpload} ảnh.`
            }
            if (totalTimes > 10) {
                var pecent = ((currentFileUpload / totalFileUpload) * 100).toFixed(2);
                progressInfo.RunBackground = true;
                progressInfo.Message = `Đang tạo Album: ${pecent}%.`;
            }
            commonJs.showProgress(progressInfo);
            if (isFinish) {
                setTimeout(function() {
                    commonJs.hideProgress();
                }, 500)
            }
        });

        state.hubConnection.on("ShowPecentDeleted", (indexFileDelete, totalFileDelete, isFinish, totalTimes, progressInfo) => {
            // Ẩn loading nếu có:
            commonJs.hideLoading();
            progressInfo = {
                Id: progressInfo.id,
                Name: progressInfo.name,
                Value: indexFileDelete,
                Max: totalFileDelete,
                Message: `Đang xóa ${indexFileDelete}/${totalFileDelete} ảnh.`
            }
            if (totalTimes > 10) {
                var pecent = ((indexFileDelete / totalFileDelete) * 100).toFixed(2);
                progressInfo.RunBackground = true;
                progressInfo.Message = `Đang xóa Album: ${pecent}%.`;
            }
            commonJs.showProgress(progressInfo);
            if (isFinish) {
                setTimeout(function() {
                    commonJs.hideProgress();
                }, 500)
            }
        });
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