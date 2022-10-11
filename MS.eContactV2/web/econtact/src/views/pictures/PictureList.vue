<template>
    <div class="album">
       <div class="album__toolbar">
        <button class="btn btn--default" id="btn-add" @click="onAddNewAlbum">Thêm mơi</button>
       </div>
       <div class="album__list"></div>
    </div>
    <picture-detail v-if="showAddNew" @onCloseAddNewDialog="showAddNew = false"></picture-detail>
</template>
<script>
import PictureDetail from './PictureDetail.vue'
export default {
    name:"PictureList",
    components:{PictureDetail},
    props:[],
    emits:[],
    methods: {
        onAddNewAlbum(){
            this.showAddNew = true;
        }
    },
    data() {
        return {
            showAddNew: true
        }
    },
}
</script>
<style scoped>

</style>