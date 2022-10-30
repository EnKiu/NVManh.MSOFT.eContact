<template>
  <m-dialog title="Thông tin liên hệ" @onClose="onClose">
    <template v-slot:content>
      <div class="contact-info">
        <div class="contact--general">
          <div
            class="contact__avatar"
            :class="{ 'edit-able': formMode != 3 }"
            @click="avatarOnClick"
            :style="{
              'background-image': `url(${contact.AvatarFullPath})`,
            }"
          >
            <!-- <img :src="contact.AvatarFullPath" alt="" /> -->
          </div>
          <div class="contact__info">
            <div class="contact__info--name">
              {{ contact.FirstName }} {{ contact.LastName }}
            </div>
            <!-- <div class="contact__info--rank"></div> -->
          </div>
          <div class="contact__phone">
            <div class="phone--main">
              <div class="phone-item">
                <div class="phone-item__number">
                  <m-input
                    class="no-border margin-0"
                    :class="{ 'bg-blue': formMode == 3 }"
                    :disabled="formMode == 3"
                    :focus="true"
                    v-model="contact.MobileNumber"
                  ></m-input>
                </div>
                <div v-if="formMode == 3" class="phone-item__call">
                  <a :href="'tel:' + contact.MobileNumber"></a>
                </div>
              </div>
              <div class="phone-item">
                <div class="phone-item__number">
                  <m-input
                    class="no-border margin-0"
                    :class="{ 'bg-yellow': formMode == 3 }"
                    :disabled="formMode == 3"
                    v-model="contact.PhoneNumber"
                  ></m-input>
                </div>
                <div v-if="formMode == 3" class="phone-item__call">
                  <a :href="'tel:' + contact.PhoneNumber"></a>
                </div>
              </div>
            </div>
            <div class="phone--ext"></div>
          </div>
        </div>
        <div class="contact--other">
          <div class="info-item social">
            <div class="social__label">
              <div class="social__icon social__icon--facebook"></div>
              <span>Facebook:</span>
            </div>
            <div class="social__info">
              <m-input
                class="no-border margin-0"
                :disabled="formMode == 3"
                :focus="true"
                v-model="contact.Facebook"
              ></m-input>
            </div>
          </div>
          <div class="info-item social">
            <div class="social__label">
              <div class="social__icon social__icon--zalo"></div>
              <span>Zalo:</span>
            </div>
            <div class="social__info">
              <m-input
                class="no-border margin-0"
                :disabled="formMode == 3"
                :focus="true"
                v-model="contact.Zalo"
              ></m-input>
            </div>
          </div>
          <div class="info-item --company">
            <div class="info__label">Nơi làm việc</div>
            <div v-if="formMode == 3">{{ contact.Workplace }}</div>
            <m-input
              class="no-border margin-0"
              v-else
              :focus="true"
              v-model="contact.Workplace"
            ></m-input>
          </div>
          <div class="info-item --address">
            <div v-if="formMode == 3">{{ contact.Address }}</div>
            <m-input
              class="no-border"
              v-else
              :focus="true"
              v-model="contact.Address"
            ></m-input>
          </div>
        </div>
      </div>
    </template>
    <template v-slot:footer>
      <button
        v-if="formMode != 3"
        class="btn btn--cancel dialog__button--cancel"
        @click="onCancel"
      >
        Hủy bỏ
      </button>
      <button class="btn btn--default" v-if="formMode == 3" @click="onChangeEditMode">
        <i class="icofont-edit-alt"></i> Sửa thông tin
      </button>
      <button
        v-if="formMode != 3"
        type="submit"
        class="btn btn--default dialog__button--save"
        @click="onSubmit"
      >
        Lưu
      </button>
      <button
        v-if="formMode == 3"
        class="btn btn--close dialog__button--close"
        @click="onClose"
      >
        Đóng
      </button>
    </template>
  </m-dialog>
  <input
    hidden
    type="file"
    ref="fileAvatar"
    name="avatar"
    @change="fileAvatarOnChange"
    id="fileAvatar"
  />
</template>
<script>
export default {
  name: "BaseDialog",
  components: {},
  emits: ["afterSave", "update:formMode"],
  props: {
    title: {
      type: String,
      default: "Thông tin chi tiết",
      required: false,
    },
    formMode: {
      type: Number,
      default: 3,
      required: false,
    },
    contactInput: {
      type: Object,
      default: function () {
        return {};
      },
      required: true,
    },
  },
  created() {
    this.contact = this.contactInput;
    this.originalContact = JSON.stringify(this.contactInput);
  },
  methods: {
    onSubmit() {
      var formData = new FormData();

      var file = this.$refs.fileAvatar.files[0];
      if (file) {
        formData.append("file", file);
      }

      formData.append("contactId", this.contact.ContactId);
      formData.append("contact", JSON.stringify(this.contact));

      // var baseUrl = process.env.VUE_APP_BASE_URL;
      // var contactId = this.contact.ContactId;
      // gọi api save dữ liệu:
      this.api({
        url: "/api/v1/contacts",
        data: formData,
        method: "PUT",
      })
        .then((res) => {
          this.$emit("afterSave", res);
        })
        .catch((res) => {
          if (res.status == 403) this.$emit("update:formMode", this.Enum.FormMode.VIEW);
          this.$emit("afterSave", res);
        });
    },
    onChangeEditMode() {
      this.$emit("update:formMode", this.Enum.FormMode.UPDATE);
    },
    onCancel() {
      this.$emit("update:formMode", this.Enum.FormMode.VIEW);
      this.contact = JSON.parse(this.originalContact);
    },
    onClose() {
      this.$emit("update:formMode", null);
    },
    avatarOnClick() {
      this.$refs.fileAvatar.click();
    },
    fileAvatarOnChange(e) {
      const name = e.target.name,
        file = e.target.files[0];
      const hasName = ["avatar"].includes(name);
      if (hasName) {
        // this.avatar = file;
        this.contact.AvatarFullPath = URL.createObjectURL(file);
      }
    },
  },
  data() {
    return {
      originalContact: "",
      contact: {
        FirstName: "Nguyễn Văn",
        LastName: "Mạnh",
        PhoneNumber: "0977340334",
        MobileNumber: "0961179969",
        Company: "Công ty cổ phần MISA",
        Address: "SN 11A, ngõ 109 Lê Lợi, TP Bắc Giang",
      },
    };
  },
};
</script>
<style scoped>
.input-wrapper {
  max-width: unset;
}

.margin-0 {
  margin: 0 !important;
}
.contact-info {
  max-width: 350px;
}
.contact--general {
  display: grid;
  grid-template-columns: 130px auto;
  grid-template-rows: auto auto;
  align-items: center;
  justify-content: space-between;
  column-gap: 10px;
}
.contact__avatar {
  width: 130px;
  height: 130px;
  border: 1px solid #ccc;
  grid-row-start: 1;
  grid-row-end: 3;
  display: flex;
  align-items: center;
  justify-content: center;
  pointer-events: none;
  background-repeat: no-repeat;
  background-position: center;
  background-size: cover;
}

.contact__avatar img {
  width: calc(100% - 2px);
  height: calc(100% - 2px);
}
.contact__info--name {
  font-size: 16px;
  font-weight: 700;
  color: #0094ff;
}

.phone-item {
  display: flex;
}

.phone-item__call {
  width: 20px;
  height: 36px;
  margin-left: 8px;
}
.phone-item__call a {
  display: block;
  width: 20px;
  height: 36px;
  flex-grow: 0;
  flex-shrink: 0;
  flex-basis: 20px;
  background-image: url(../../assets/icon/call_phone.png);
  background-size: contain;
  background-repeat: no-repeat;
  background-position: center;
  opacity: 0.7;
}

.phone-item__call a:hover {
  opacity: 1;
}
.phone-item + .phone-item {
  margin-top: 10px;
}

.social {
  display: flex;
  align-items: center;
}
.social__label,
.info__label {
  display: flex;
  align-items: center;
  width: 120px;
}

.info-item {
  border-bottom: 1px solid #ccc;
  padding: 10px 0;
}

.info-item .input-wrapper {
  flex: 1;
}

.info__label + * {
  flex: 1;
}
.contact--other input {
  width: 100%;
}

.social__icon {
  width: 32px;
  height: 32px;
  flex-grow: 0;
  flex-shrink: 0;
  flex-basis: 32px;
  margin-right: 10px;
  background-size: contain;
  background-repeat: no-repeat;
  background-position: center;
}

.social__icon--facebook {
  background-image: url(../../assets/icon/facebook.png);
}
.social__icon--zalo {
  background-image: url(../../assets/icon/zalo.png);
}
.social__info {
  flex: 1;
}

.info-item {
  display: flex;
}

button + button {
  margin-left: 16px;
}
.edit-able {
  border: 1px solid #0094ff;
  cursor: pointer;
  pointer-events: all !important;
}
/* .--address>div,.--company>div{
    max-width: 200px;
} */
</style>
