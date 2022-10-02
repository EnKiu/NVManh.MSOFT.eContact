import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import commonJs from './scripts/common';
import Enum from './scripts/enum';
import MInput from './components/base/MInput.vue'

const app = createApp(App);
app.component("MInput", MInput);
app.config.globalProperties.commonJs = commonJs;
app.config.globalProperties.Enum = Enum;

app.use(router)
app.mount('#app')