<template>
  <m-dialog title="Thêm mới sự kiện" @onClose="$emit('onClose')">
    <template v-slot:content>
      <div class="event">
        <div class="event--info">
          <div class="event__title">
            <m-input
              label="Tiêu đề sự kiện"
              :required="true"
              :validated="validated"
              @onValidate="setValidOnInputRequired"
              v-model="event.EventName"
            ></m-input>
          </div>
          <div class="event__date">
            <label for="">Thời gian tổ chức:</label>
            <el-date-picker
              v-model="event.StartTime"
              type="datetime"
              format="DD-MM-YYYY HH:mm:ss"
              placeholder="Chọn ngày giờ tổ chức"
            />
          </div>
          <div class="event__expire-date">
            <label for="">Hạn đăng ký:</label>
            <el-date-picker
              v-model="event.ExpireRegisterDate"
              type="date"
              format="DD-MM-YYYY HH:mm:ss"
              placeholder="Chọn ngày giờ hết hạn"
            />
          </div>
          <div class="event__place">
            <m-input label="Địa điểm" v-model="event.EventPlace"></m-input>
          </div>
          <div class="event__spend">
            <m-input label="Kinh phí dự kiến/người" v-model="event.Spends"></m-input>
          </div>
        </div>
        <div class="event--content">
          <button id="show-content" @click="onShowEditorContent">
            <i class="icofont-swoosh-right"></i> Nhập nội dung sự kiện
          </button>
          <div v-if="showContenEditor" class="content-editor">
            <div class="editor">
              <button
                class="close-editor"
                @click="onCloseEditorContent"
                title="Đóng Form"
              >
                <i class="icofont-close"></i>
              </button>
              <ckeditor
                :editor="editor"
                v-model="event.EventContent"
                :config="editorConfig"
              ></ckeditor>
              <div class="editor__button">
                <button
                  id="save-editor"
                  class="btn btn--default"
                  @click="showContenEditor = false"
                >
                  <i class="icofont-check"></i> Lưu
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </template>
    <template v-slot:footer>
      <div class="footer-button">
        <button id="btn-save" class="btn btn--default" @click="onSave">
          <i class="icofont-check"></i> Lưu
        </button>
        <button class="btn dialog__button--cancel" @click="$emit('onClose')">
          <i class="icofont-ui-close"></i> Đóng
        </button>
      </div>
    </template>
  </m-dialog>
</template>
<script>
import ClassicEditor from "@ckeditor/ckeditor5-build-classic";
import Enum from "@/scripts/enum";
export default {
  name: "EventDetail",
  components: {},
  props: ["eventItem", "formMode"],
  emits: ["onClose", "onSaveSuccess"],
  created() {
    console.log(this.eventItem);
    if (this.formMode == Enum.FormMode.UPDATE) {
      this.event = this.eventItem;
    }
  },
  methods: {
    onSave() {
      this.isValid = this.doValidate();
      if (!this.isValid) return;
      else {
        var method = "POST";
        var url = "/api/v1/events";
        this.event.EventDate = this.event.StartTime;
        if (this.formMode == Enum.FormMode.UPDATE) {
          method = "PUT";
          url += `/${this.eventItem.EventId}`;
        }
        this.api({
          url: url,
          data: this.event,
          method: method,
        }).then(() => {
          this.$emit("onSaveSuccess");
        });
      }
    },
    setValidOnInputRequired(isValid) {
      this.isValid = isValid;
    },
    doValidate() {
      this.validated = true;
      if (!this.event.EventName) {
        this.commonJs.showMessenger({
          title: "Dữ liệu không hợp lệ",
          msg: "Tên sự kiện không được phép trống, vui lòng kiểm tra lại.",
          type: Enum.MsgType.Error,
        });
        return false;
      } else {
        return true;
      }
    },
    /**
     * Hiển thị form nhập nội dung sự kiện
     * Author: NVMANH (11/10/2022)
     */
    onShowEditorContent() {
      this.showContenEditor = true;
      this.eventContentOrginal = this.event.EventContent;
    },

    /**
     * Kiểm tra sự thay đổi của nội dung trước khi đóng form
     * Author: NVMANH (11/10/2022)
     */
    onCloseEditorContent() {
      if (this.eventContentOrginal != this.event.EventContent) {
        this.commonJs.showConfirm(
          "Nội dung sự kiện đã bị thay đổi, bạn có chắc chắn muốn hủy thay đổi?",
          () => {
            this.eventContentOrginal = this.event.EventContent;
            this.showContenEditor = false;
          }
        );
      } else {
        this.showContenEditor = false;
      }
    },
  },
  data() {
    return {
      isValid: true,
      validated: false,
      event: {},
      editor: ClassicEditor,
      editorData: "<p>Content of the editor.</p>",
      editorConfig: {
        // The configuration of the editor.
        language: "vn",
        toolbar: {
          items: [
            "heading",
            "|",
            "bold",
            "italic",
            "|",
            "link",
            "|",
            "bulletedList",
            "numberedList",
            "|",
            "insertTable",
            "|",
            // "uploadImage",
            "blockQuote",
            "|",
            "undo",
            "redo",
          ],
        },
      },
      showContenEditor: false,
      eventContentOrginal: "",
    };
  },
};
</script>
<style scoped>
.event .input-wrapper {
  margin: 0;
}

.event .event--content {
  padding: 10px 0;
}
.ck.ck-editor__main > .ck-editor__editable {
  height: 500px !important;
}
.event {
  position: relative;
  max-width: 400px;
  width: 100%;
  box-sizing: border-box;
}
.event--info {
  display: grid;
  row-gap: 16px;
  width: 100%;
  border-radius: 4px;
  box-sizing: border-box;
  clear: both;
}

.event--content {
  width: 100%;
  float: left;
  box-sizing: border-box;
  text-align: right;
  padding: 24px;
  box-sizing: border-box;
}
.content-editor {
  max-width: 100%;
  padding: 24px 30px;
}
.footer-button {
  display: flex;
}
#btn-save {
  display: flex;
  align-items: center;
  order: 1;
  margin-left: 10px;
}
#btn-save i {
  font-size: 24px;
}
#show-content {
  color: #3395ff;
  cursor: pointer;
  border: unset;
  background: none;
}

#show-content:hover {
  text-decoration: underline;
}
.editor__button {
  width: 100%;
  margin-top: 10px;
  display: flex;
  justify-content: flex-end;
}
#save-editor {
  display: flex;
  align-items: center;
}
#save-editor i {
  font-size: 24px;
}
.content-editor {
  position: fixed;
  top: 0;
  left: 0;
  bottom: 0;
  right: 0;
  background-color: #0000003c;
  display: flex;
  align-items: center;
  justify-content: center;
}
.editor {
  max-width: 100%;
  position: relative;
  padding: 24px;
  background-color: #fff;
  border-radius: 4px;
}
button.close-editor {
  position: absolute;
  top: -10px;
  right: -10px;
  border-radius: 50%;
  width: 30px;
  height: 30px;
  font-size: 24px;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  color: #ff0000;
  border-color: #ff0000;
  outline: none;
  border-style: solid;
}
</style>
