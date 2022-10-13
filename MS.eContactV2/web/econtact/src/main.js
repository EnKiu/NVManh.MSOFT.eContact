import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import commonJs from './scripts/common';
import api from './utils/api'
import Enum from './scripts/enum';
import MInput from './components/base/MInput.vue'
import MButton from './components/base/MButton.vue'
import MButtonIcon from './components/base/MButtonIcon.vue'
import MButtonFontIcon from './components/base/MButtonFontIcon.vue'
import MTextArea from './components/base/MTextArea.vue'
import MCombobox from './components/base/MCombobox.vue'
import MDialog from './components/base/MDialog.vue'
import MLoading from './components/base/MLoading.vue'
import MToast from './components/base/MToast.vue'
import 'element-plus/dist/index.css'
import 'element-plus/es/components/loading/style/css'
import 'element-plus/es/components/date-picker/style/css'
import locale from 'element-plus/lib/locale/lang/vi'
import ElementPlus from 'element-plus'
import CKEditor from '@ckeditor/ckeditor5-vue'
import { ElTable, ElTableColumn } from 'element-plus'

const app = createApp(App);
app.component("MInput", MInput);
app.component("MButton", MButton);
app.component("MTextArea", MTextArea);
app.component("MCombobox", MCombobox);
app.component("MButtonIcon", MButtonIcon);
app.component("MButtonFontIcon", MButtonFontIcon);
app.component("MDialog", MDialog);
app.component("MToast", MToast);
app.component("MLoading", MLoading);
app.component("MTable", ElTable);
app.component("MColumn", ElTableColumn);

app.config.globalProperties.commonJs = commonJs;
app.config.globalProperties.api = api;
app.config.globalProperties.Enum = Enum;

document.addEventListener("DOMContentLoaded", function() {
    var elements = document.getElementsByTagName("INPUT");
    for (var i = 0; i < elements.length; i++) {
        elements[i].oninvalid = function(e) {
            e.target.setCustomValidity("");
            if (!e.target.validity.valid) {
                e.target.setCustomValidity("Thông tin này không được phép để trống");
            }
        };
        elements[i].oninput = function(e) {
            e.target.setCustomValidity("");
        };
    }
})

app.use(ElementPlus, {
    locale: locale,
})
app.use(store).use(router).use(CKEditor)
app.mount('#app')