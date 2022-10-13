import { createRouter, createWebHistory } from 'vue-router'
import ContactList from '../views/contacts/ContactList.vue'
import EventList from '../views/events/EventList.vue'
// import NewList from '../views/news/NewList.vue'
import AlbumList from '../views/pictures/AlbumList.vue'
import LoginPage from '../views/account/Login.vue'
import Register from '../views/account/Register.vue'
const routes = [{
        path: '/',
        name: 'HomePage',
        component: ContactList,
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
    }
]

const router = createRouter({
    history: createWebHistory(process.env.BASE_URL),
    routes
})

export default router