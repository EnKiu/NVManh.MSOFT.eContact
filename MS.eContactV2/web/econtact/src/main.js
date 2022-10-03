import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import commonJs from './scripts/common';
import Enum from './scripts/enum';
import MInput from './components/base/MInput.vue'
import MDialog from './components/base/MDialog.vue'
import 'element-plus/dist/index.css'
import 'element-plus/es/components/loading/style/css'
import 'element-plus/es/components/date-picker/style/css'
import locale from 'element-plus/lib/locale/lang/vi'
import ElementPlus from 'element-plus'
import CKEditor from '@ckeditor/ckeditor5-vue'

const app = createApp(App);
app.component("MInput", MInput);
app.component("MDialog", MDialog);
app.config.globalProperties.commonJs = commonJs;
app.config.globalProperties.Enum = Enum;

app.use(ElementPlus, {
    locale: locale,
})
app.use(router).use(CKEditor)
app.mount('#app')