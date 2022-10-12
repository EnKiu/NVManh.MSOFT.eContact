<template>
  <div class="album">
    <div class="album__toolbar">
      <button class="btn btn--default" id="btn-add" @click="onAddNewAlbum">
        <i class="icofont-ui-rate-add"></i> Tạo Album mới
      </button>
    </div>
    <div class="album__list">
      <div
        class="album-item"
        v-for="(album, index) in albums"
        :key="index"
        @click="showDetailAlbum(album)"
      >
        <button class="btn--remove-item" @click="onRemoveAlbum(album,index)" title="Xóa Album">
          <i class="icofont-ui-remove"></i>
        </button>
        <div class="album__title">{{ album.AlbumName }}</div>
        <div class="album__date">
          Ngày tạo: {{ commonJs.formatDate(album.CreatedDate) }}
        </div>
        <div class="album__total-pictures">
          Tổng số ảnh: {{ album.TotalPictures }}
        </div>
        <div class="album__total-view">
          Tổng số lượt xem: {{ album.TotalViews }}
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
  },
  methods: {
    onAddNewAlbum() {
      this.showAddNew = true;
    },
    onAfterAddAlbum() {
      this.showAddNew = false;
      this.loadAlbum();
    },
    onRemoveAlbum(album,index){
      console.log(album);
      console.log("album-index:",index);
    },
    showDetailAlbum(album) {
      this.albumSelected = album;
    },
    loadAlbum() {
      this.api({
        url: "/api/v1/Albums",
      }).then((res) => {
        console.log(res);
        this.albums = res;
      });
    },
  },
  data() {
    return {
      showAddNew: false,
      albums: [],
      albumSelected: null,
    };
  },
};
</script>
<style scoped>
.album {
  padding: 0 16px 16px 16px;
  box-sizing: border-box;
  max-width: 700px;
}

.album * {
  box-sizing: border-box;
}
.album__toolbar {
  padding: 0 10px;
}
.album__list {
  max-width: 1366px;
  flex-wrap: wrap;
}
.album-item {
  position: relative;
  min-width: 300px;
  float: left;
  max-width: 100%;
  border: 1px solid #dedede;
  border-radius: 4px;
  box-shadow: 0 3px 6px #404040;
  padding: 16px;
  margin: 10px;
  cursor: pointer;
}
.album-item .btn--remove-item{
  display: none;
  color: #ff0000;
  border-color: #ff0000;
  border-style: solid;
  border:none
}
.album-item:hover .btn--remove-item{
  display: flex;
}

.album__title {
  font-size: 16px;
  font-weight: 700;
}
</style>
