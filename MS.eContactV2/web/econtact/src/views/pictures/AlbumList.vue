<template>
  <div class="album">
    <div class="album__toolbar">
      <button class="btn btn--default" id="btn-add" @click="onAddNewAlbum">
        <i class="icofont-ui-rate-add"></i> Tạo Album mới
      </button>
    </div>
    <div class="album__list">
      <!-- <m-table
        ref="tbListDocument"
        :data="albums"
        empty-text="Không có dữ liệu"
        @row-click="showDetailAlbum"
        width="100%"
        height="100%"
      >
        <m-column prop="FullName" class="album-item">
          <template #default="scope">
            <div class="album-item">
              <button
                class="btn--remove-item"
                @click.prevent="onRemoveAlbum(scope.row, index)"
                title="Xóa Album"
              >
                <i class="icofont-ui-remove"></i>
              </button>
              <div class="album__title">{{ scope.row.AlbumName }}</div>
              <div class="album__date">
                <i class="icofont-ui-clock"></i> Ngày tạo:
                {{ commonJs.formatDate(scope.row.CreatedDate) }}
              </div>
              <div class="album__total-pictures">
                <i class="icofont-files-stack"></i>Tổng số ảnh:
                {{ scope.row.TotalPictures }}
              </div>
              <div class="album__total-view">
                <i class="icofont-eye-alt"></i>Tổng số lượt xem:
                {{ scope.row.TotalViews }}
              </div>
            </div>
          </template>
        </m-column>
      </m-table> -->
      <!-- BẢNG CŨ -->
      <div
        class="album-item"
        v-for="(album, index) in albums"
        :key="index"
        @click="showDetailAlbum(album)"
      >
        <button
          v-if="isAdmin"
          class="btn--remove-item"
          @click.prevent="onRemoveAlbum(album, index)"
          title="Xóa Album"
        >
          <i class="icofont-ui-remove"></i>
        </button>
        <div class="album__title">{{ album.AlbumName }}</div>
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
        Đã xóa <b>{{ indexFileDelete }}/{{ totalFileDelete }}</b> ảnh trong Album.
      </div>
    </div>
  </div>
</template>
<script>
import AlbumItem from "./AlbumItem.vue";
import AlbumDetail from "./AlbumDetail.vue";
export default {
  name: "AlbumList",
  components: { AlbumItem, AlbumDetail },
  props: [],
  emits: [],
  created() {
    this.loadAlbum();
    var roleValue = localStorage.getItem("userRoleValue");
    if (roleValue == 1) {
      this.isAdmin = true;
    }
  },
  methods: {
    onAddNewAlbum() {
      this.showAddNew = true;
    },
    onAfterAddAlbum() {
      this.showAddNew = false;
      this.loadAlbum();
    },
    onRemoveAlbum(album) {
      this.commonJs.showConfirm("Bạn có chắc chắn muốn xóa Album này không?", () => {
        this.totalFileDelete = album.TotalPictures;
        this.api({ url: "api/v1/albums/" + album.AlbumId, method: "DELETE" }).then(() => {
          this.loadAlbum();
        });
      });
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
