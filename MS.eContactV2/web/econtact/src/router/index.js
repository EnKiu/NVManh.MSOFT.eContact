import { createRouter, createWebHistory } from 'vue-router'
import ContactList from '../views/contacts/ContactList.vue'
import EventList from '../views/events/EventList.vue'
// import NewList from '../views/news/NewList.vue'
import AlbumList from '../views/pictures/AlbumList.vue'
import LoginPage from '../views/account/Login.vue'
import Register from '../views/account/Register.vue'
import AccountInfo from '../views/account/AccountInfo.vue'
import HomePage from '../views/Index.vue'
const routes = [{
        path: '/',
        name: 'HomePage',
        components: {
            HomePage: HomePage
        },
    },
    {
        path: '/login',
        name: 'LoginPage',
        components: {
            LoginPage: LoginPage
        }
    },
    {
        path: '/register',
        name: 'Register',
        components: {
            Register: Register
        }
    },
    {
        path: '/contacts',
        name: 'ContactList',
        component: ContactList
    },
    {
        path: '/events',
        name: 'EventList',
        component: EventList
    },
    {
        path: '/pictures',
        name: 'AlbumList',
        component: AlbumList
    },
    {
        path: '/account',
        name: 'AccountInfo',
        component: AccountInfo
    }
]

const router = createRouter({
    history: createWebHistory(process.env.BASE_URL),
    routes
})

export default router