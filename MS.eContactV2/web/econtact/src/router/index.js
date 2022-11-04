import { createRouter, createWebHistory } from 'vue-router'
import ContactList from '../views/contacts/ContactList.vue'
import store from '@/store'
import EventList from '../views/events/EventList.vue'
// import NewList from '../views/news/NewList.vue'
import AlbumList from '../views/pictures/AlbumList.vue'
import ExpenditureIndex from '../views/expenditure/Index.vue'
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
    {
        path: '/expenditures',
        name: 'ExpenditureIndex',
        component: ExpenditureIndex,
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
    }
]

const router = createRouter({
    history: createWebHistory(process.env.BASE_URL),
    routes
})

export default router