import {
    SET_ERROR,
    POST_ERROR,
    CLEAR_ERROR
} from "../actions/error";

const state = {
    // isError: true,
    errors: []
};

const getters = {
    isError: state => !!(state.errors.length > 0),
    errors: function() {
        return state.errors;
    }
};

const actions = {
    [SET_ERROR]: ({ commit }, errorMsg) => {
        commit("POST_ERROR", errorMsg);
    },
    [CLEAR_ERROR]: ({ commit }) => {
        commit("CLEAR_ERROR");
    },
};

const mutations = {
    [POST_ERROR]: (state, payload) => {
        for (const item of payload) {
            if (typeof item === 'string' || item instanceof String || typeof itemChild == 'number') {
                state.errors.push(item);
            } else if (Array.isArray(item)) {
                for (const itemChild of item) {
                    if (typeof itemChild === 'string' || itemChild instanceof String || typeof itemChild == 'number') {
                        state.errors.push(itemChild.toString());
                    }
                }
            }
        }
    },
    [CLEAR_ERROR]: (state) => {
        state.errors = [];
    },
};

export default {
    state,
    getters,
    actions,
    mutations
};