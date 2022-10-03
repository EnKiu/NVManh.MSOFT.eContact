import { createRouter, createWebHistory } from 'vue-router'
import ContactList from '../views/contacts/ContactList.vue'
import EventList from '../views/events/EventList.vue'
// import NewList from '../views/news/NewList.vue'
import PictureList from '../views/pictures/PictureList.vue'
const routes = [{
        path: '/',
        name: 'ContactList',
        component: ContactList,
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
        name: 'PictureList',
        component: PictureList
    }
]

const router = createRouter({
    history: createWebHistory(process.env.BASE_URL),
    routes
})

export default router