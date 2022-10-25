 
<template>
  <div class="album">
    <div class="album__toolbar">
      <button class="btn btn--default" id="btn-add" @click="onAddNewAlbum">
        <i class="icofont-ui-rate-add"></i> Tạo Album mới
      </button>
    </div>
    <div class="album__list">
      <!-- BẢNG XEM TRƯỚC KHI THỰC HIỆN THÊM MỚI ĐANG TRONG TRẠNG THÁI CHỜ -->
      <div v-if="creating" class="album-item">
        <div class="album__title">(Đang tạo... {{ progressPecent }})</div>
        <div class="album__date">
          <i class="icofont-ui-clock"></i> Ngày tạo: Đang hoàn thiện...
        </div>
        <div class="album__total-pictures">
          <i class="icofont-files-stack"></i>Tổng số ảnh: Đang hoàn thiện...
        </div>
        <div class="album__total-view">
          <i class="icofont-eye-alt"></i>Tổng số lượt xem: Đang hoàn thiện...
        </div>
      </div>

      <!-- DANH SÁCH ALBUM -->
      <div
        class="album-item"
        v-for="(album, index) in albums"
        :key="index"
        @click="showDetailAlbum(album)"
      >
        <button
          v-if="isAdmin && !album.IsDeleting"
          class="btn--remove-item"
          @click.prevent="onRemoveAlbum(album, index)"
          title="Xóa Album"
        >
          <i class="icofont-ui-remove"></i>
        </button>
        <div class="album__title">{{ album.AlbumName }} <span v-if="album.IsDeleting">(Đang xóa...)</span></div>
        <div class="album__date">
          <i class="icofont-ui-clock"></i> Ngày tạo:
          {{ commonJs.formatDate(album.CreatedDate) }}
        </div>
        <div class="album__total-pictures">
          <i class="icofont-files-stack"></i>Tổng số ảnh:
          {{ album.TotalPictures }}
        </div>
        <div class="album__total-view">
          <i class="icofont-eye-alt"></i>Tổng số lượt xem:
          {{ album.TotalViews }}
        </div>
      </div>
    </div>
  </div>
  <album-item
    v-if="showAddNew"
    @onAddAlbum="creating = true"
    @onCloseAddNewDialog="showAddNew = false"
    @afterAddAlbum="onAfterAddAlbum"
  ></album-item>
  <album-detail
    v-if="albumSelected"
    :album="albumSelected"
    @onCloseAlbumDetail="albumSelected = null"
    v-model:totalViews="albumSelected.TotalViews"
    :key="albumSelected.AlbumId"
  ></album-detail>
  <div v-if="deleting" class="uploading">
    <div class="uploading__content">
      <progress :value="indexFileDelete" :max="totalFileDelete"></progress>
      <div class="uploading__info">
        Đã xóa <b>{{ indexFileDelete }}/{{ totalFileDelete }}</b> ảnh trong
        Album.
      </div>
    </div>
  </div>
</template>
<script>
/* eslint-disable */
import AlbumItem from "./AlbumItem.vue";
import AlbumDetail from "./AlbumDetail.vue";
import { mapGetters } from "vuex";
import { forIn } from "lodash";
export default {
  name: "AlbumList",
  components: { AlbumItem, AlbumDetail },
  props: [],
  emits: [],
  computed: {
    ...mapGetters(["processList"]),
  },
  created() {
    console.log(this.processList);
    this.loadAlbum();
    var roleValue = localStorage.getItem("userRoleValue");
    if (roleValue == 1) {
      this.isAdmin = true;
    }
    this.hubConnection.on(
      "ShowPecentUpload",
      (
        currentFileUpload,
        totalFileUpload,
        isFinish,
        totalTimes,
        progressInfo
      ) => {
        if (totalTimes > 10) {
          this.showAddNew = false;
          this.creating = true;
          this.progressPecent =
            Math.round(
              ((currentFileUpload / totalFileUpload) * 100).toFixed(2)
            ) + "%";
        }
        if (isFinish) {
          this.creating = false;
          this.loadAlbum();
        }
      }
    );
    this.hubConnection.on(
      "ShowPecentDeleted",
      (
        indexFileDelete,
        totalFileDelete,
        isFinish,
        totalTimes,
        progressInfo
      ) => {
        
        for (const album of this.albums) {
          if (album.AlbumId == progressInfo.id) {
            console.log(true);
            album.IsDeleting = true;
          }
        }
        if (isFinish) {
          this.loadAlbum();
        }
      }
    );
  },
  methods: {
    onAddNewAlbum() {
      this.showAddNew = true;
    },
    onAfterAddAlbum() {
      this.showAddNew = false;
      this.creating = false;
      this.loadAlbum();
    },
    onRemoveAlbum(album) {
      this.commonJs.showConfirm(
        "Bạn có chắc chắn muốn xóa Album này không?",
        () => {
          this.totalFileDelete = album.TotalPictures;
          this.api({
            url: "api/v1/albums/" + album.AlbumId,
            method: "DELETE",
            showToast: false,
            showMsg: false,
          }).then(() => {
            this.loadAlbum();
          });
        }
      );
      event.stopPropagation();
    },
    showDetailAlbum(album) {
      this.albumSelected = album;
    },
    loadAlbum() {
      this.api({
        url: "/api/v1/Albums",
      }).then((res) => {
        this.albums = res;
      });
    },
  },
  data() {
    return {
      showAddNew: false,
      albums: [],
      albumSelected: null,
      deleting: false,
      indexFileDelete: 0,
      totalFileDelete: 0,
      isAdmin: false,
      creating: false,
      progressPecent: "0%",
    };
  },
};
</script>
<style scoped>
.album {
  box-sizing: border-box;
  max-width: 700px;
  height: 100%;
  box-sizing: border-box;
}

.album * {
  box-sizing: border-box;
}
.album__toolbar {
  padding: 0 10px;
}
.album__list {
  max-width: 1366px;
  display: flex;
  flex-wrap: wrap;
  height: calc(100% - 46px);
  overflow-y: auto;
  margin-top: 10px;
}
.album-item {
  position: relative;
  min-width: 250px;
  float: left;
  max-width: 100%;
  border: 1px solid #dedede;
  border-radius: 4px;
  box-shadow: 0 0 10px #404040;
  padding: 16px;
  margin: 10px;
  cursor: pointer;
  flex: 1;
}

.album-item div + div {
  margin-top: 10px;
}

.album-item div i {
  margin-right: 10px;
}
.album-item .btn--remove-item {
  display: flex;
  color: #ff0000;
  border-color: #ff0000;
  border-style: solid;
  border: none;
  z-index: 0;
}
/* .album-item:hover .btn--remove-item {
  display: flex;
} */

.album__title {
  font-size: 16px;
  font-weight: 700;
  color: #0059ff;
  background: -webkit-linear-gradient(rgb(255, 0, 0), #0084ff);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  transition: all 1000ms linear;
}
</style>
