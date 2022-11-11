import { createRouter, createWebHistory } from 'vue-router'
import ContactList from '../views/contacts/ContactList.vue'
import store from '@/store'
import EventList from '../views/events/EventList.vue'
// import NewList from '../views/news/NewList.vue'
import AlbumList from '../views/pictures/AlbumList.vue'

// THU CHI
import ExpenditureIndex from '../views/funds/Index.vue'
import ExpenditurePlanDetail from '../views/funds/plans/ExpenditurePlanDetail.vue'
import ExpenditureDetail from '../views/funds/expenditures/ExpenditureDetail.vue'

import LoginPage from '../views/account/Login.vue'
import Register from '../views/account/Register.vue'
import AccountInfo from '../views/account/AccountInfo.vue'
import HomePage from '../views/Index.vue'

const ifNotAuthenticated = (to, from, next) => {
    if (!store.getters.isAuthenticated) {
        next();
        return;
    }
    next("/contacts");
};

const ifAuthenticated = (to, from, next) => {
    if (store.getters.isAuthenticated) {
        next();
        return;
    }
    localStorage.clear();
    next("/login");
};


const routes = [{
        path: '/',
        name: 'HomePage',
        components: {
            HomePage: HomePage
        },
        beforeEnter: ifAuthenticated
    },
    {
        path: '/login',
        name: 'LoginPage',
        components: {
            LoginPage: LoginPage
        },
        beforeEnter: ifNotAuthenticated
    },
    {
        path: '/register',
        name: 'Register',
        components: {
            Register: Register
        },
        beforeEnter: ifNotAuthenticated
    },
    {
        path: '/contacts',
        name: 'ContactList',
        component: ContactList,
        beforeEnter: ifAuthenticated
    },
    // {
    //     path: '/expenditures',
    //     name: 'ExpenditureIndex',
    //     component: ExpenditureIndex,
    //     beforeEnter: ifAuthenticated
    // },

    // {
    //     path: '/funds/plans/create',
    //     name: 'ExpenditureList',
    //     components: { ExpenditureDialog: ExpenditurePlanDetail },
    //     props: route => ({ type: route.query.type, tab: route.query.tab }),
    //     beforeEnter: ifAuthenticated
    // },
    // {
    //     path: '/funds/plans/:id',
    //     name: 'ExpenditureList',
    //     components: { ExpenditureDialog: ExpenditurePlanDetail },
    //     props: true,
    //     beforeEnter: ifAuthenticated
    // },
    // {
    //     path: '/funds/:id',
    //     name: 'ExpenditureList',
    //     components: { ExpenditureDialog: ExpenditureDetail },
    //     props: route => ({ type: route.query.type, tab: route.query.tab }),
    //     beforeEnter: ifAuthenticated
    // },
    // {
    //     path: '/funds/create',
    //     name: 'ExpenditureList',
    //     components: { ExpenditureDialog: ExpenditureDetail },
    //     props: route => ({ type: route.query.type, tab: route.query.tab }),
    //     beforeEnter: ifAuthenticated
    // },
    {
        path: '/funds',
        name: 'ExpenditureList',
        component: ExpenditureIndex,
        props: route => ({ type: route.query.type, tab: route.query.tab }),
        children: [{
                path: 'plans',
                children: [{
                        path: "create",
                        components: { ExpenditureDialog: ExpenditurePlanDetail },
                    },
                    {
                        path: ":id",
                        components: { ExpenditureDialog: ExpenditurePlanDetail },
                        props: true
                    }
                ],
                // components: { Test: ExpenditurePlanDetail },
            }, {
                path: 'create',
                components: { ExpenditureDialog: ExpenditureDetail },
            },
            {
                path: ':id',
                components: { ExpenditureDialog: ExpenditureDetail },
                props: true
            }
        ],

        beforeEnter: ifAuthenticated
    },
    {
        path: '/events',
        name: 'EventList',
        component: EventList,
        beforeEnter: ifAuthenticated
    },
    {
        path: '/pictures',
        name: 'AlbumList',
        component: AlbumList,
        beforeEnter: ifAuthenticated
    },
    {
        path: '/account',
        name: 'AccountInfo',
        component: AccountInfo,
        beforeEnter: ifAuthenticated
    },

]

const router = createRouter({
    history: createWebHistory(process.env.BASE_URL),
    routes
})

export default router