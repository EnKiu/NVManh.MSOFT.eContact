import {
    CREATE_CONNECTION,
    CONNECT_SIGNALR_HUB,
    SET_NEW_NOTICE_NUMBER,
    CLEAR_NEW_NOTICE_NUMBER,
    SET_CONNECTING_HUB,
    SET_NEW_HUB_CONNECTION,
    DISCONECT_HUB,
    UPDATE_NEW_HUB_SESSION,
    UPDATE_CLASS_INFO,
} from "../actions/signalR";

import webSocket from '../../http/WebSocket';
import commonJs from "@/scripts/common";
import apiCall from "@/utils/api";

const state = {
    hubConnection: null,
    // isError: true,
    newNotifications: 0,
    isConnecting: false,
    classInfo: {
        TotalMembers: 0,
        TotalMoneys: 0
    }
};


const getters = {
    newNotifications: state => !!(state.newNotifications),
    hubConnection: state => state.hubConnection,
    connectingHub: state => state.isConnecting,
    classInfo: state => state.classInfo,
};

const actions = {
    [CREATE_CONNECTION]: ({ commit }) => {
        commit("CREATE_CONNECTION");
    },
    [CONNECT_SIGNALR_HUB]: ({ commit }) => {
        commit("CONNECT_SIGNALR_HUB");
    },
    [SET_NEW_NOTICE_NUMBER]: ({ commit }, { number }) => {
        commit("SET_NEW_NOTICE_NUMBER", { number });
    },
    [CLEAR_NEW_NOTICE_NUMBER]: ({ commit }) => {
        commit("CLEAR_NEW_NOTICE_NUMBER");
    },
    [SET_CONNECTING_HUB]: ({ commit }, isConnecting) => {
        commit("SET_CONNECTING_HUB", isConnecting);
    },
    [SET_NEW_HUB_CONNECTION]: ({ commit }, hubConnection) => {
        if (!state.hubConnection && !hubConnection) {
            commit("CREATE_CONNECTION");
        }
        commit("SET_NEW_HUB_CONNECTION", hubConnection);
        // await commit("INITIALIZATION_HUB");
        // Lấy dữ liệu cần thiết:
        // await state.hubConnection.invoke("GetClassInfo");
    },
    [DISCONECT_HUB]: ({ commit }, hubConnection) => {
        commit("DISCONECT_HUB", hubConnection);
    },
    [UPDATE_NEW_HUB_SESSION]: ({ commit }) => {
        commit("UPDATE_NEW_HUB_SESSION");
    },
    [UPDATE_CLASS_INFO]: ({ commit }, classInfo) => {
        if (!classInfo) {
            apiCall({ url: "api/v1/accounts/class-info", showToast: false })
                .then(res => {
                    classInfo = res;
                    commit("UPDATE_CLASS_INFO", classInfo);
                })
        } else
            commit("UPDATE_CLASS_INFO", classInfo);
    }
};

const mutations = {
    [CREATE_CONNECTION]: () => {
        var hubConnection = webSocket.createHub();
        hubConnection
            .start()
            .then(() => {
                console.log("Đã kết nối tới Hub...");
                commonJs.hideConnectingHub();
            })
            .catch((err) => {
                console.error(err);
                commonJs.hideConnectingHub();
            });
        state.hubConnection = hubConnection;
    },
    [CONNECT_SIGNALR_HUB]: () => {},
    [SET_NEW_HUB_CONNECTION]: (state, hubConnection) => {
        if (hubConnection) {
            state.hubConnection = hubConnection;
        }
    },
    "INITIALIZATION_HUB": () => {
        if (!state.hubConnection._methods["updateclassinfo".toLocaleLowerCase()]) {
            state.hubConnection.on("UpdateClassInfo", (classInfo) => {
                state.classInfo = classInfo;
                console.log("Đã lấy thông tin lớp", classInfo);
            })
        }


        if (!state.hubConnection._methods["ReceiveNotificationWhenDisconnected".toLocaleLowerCase()]) {
            state.hubConnection.on("ReceiveNotificationWhenDisconnected", (username) => {
                console.log(`${username} đã ngắt kết nối!`);
            })
        }


        if (!state.hubConnection._methods["ShowAlertWhenOnline".toLocaleLowerCase()]) {
            state.hubConnection.on("ShowAlertWhenOnline", (username) => {
                console.log(`${username} đã kết nối!`);
            })
        }


        if (!state.hubConnection._methods["ShowPecentUpload".toLocaleLowerCase()]) {
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
        }

        if (!state.hubConnection._methods["ShowPecentDeleted".toLocaleLowerCase()]) {
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
        }
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
    [DISCONECT_HUB]: (state) => {
        console.log(state.hubConnection);
        console.log("DISCONECT_HUB");
        state.hubConnection.stop();
    },
    [UPDATE_NEW_HUB_SESSION]: () => {
        console.log("UPDATE_NEW_HUB_SESSION");
    },
    [UPDATE_CLASS_INFO]: (state, classInfo) => {
        state.classInfo = classInfo;
        document.title = classInfo.Description;
    }
};

export default {
    state,
    getters,
    actions,
    mutations
};