<template>
    <div>
        <div class="expenditure__detail">
            <m-table
            ref="tbListExpenditure"
            :data="expenditures"
            empty-text="Không có dữ liệu"
            width="100%"
            height="100%"
          >
            <m-column prop="Amount" label="Thu/Chi">
              <template #default="scope">
                <span v-if="isReduce(scope.row.ExpenditureType)" class="expenditure-reduce">-</span>
                <span v-else class="expenditure-increment">+</span>
                <span class="expenditure-amount">{{commonJs.formatMoney(scope.row.Amount)}}</span>
              </template>
            </m-column>
           
            <m-column label="Ngày" prop="ExpenditureDate" width="100">
              <template #default="scope">
                <span>{{ commonJs.formatDate(scope.row.ExpenditureDate) }}</span>
              </template>
            </m-column>
            <m-column label="Mô tả" prop="Description"></m-column>
          </m-table>
        </div>
    </div>
</template>
<script>
export default {
    name:"ExpenditureList",
    components:{},
    emits:[],
    props:[],
    created() {
        this.loadData();
    },
    methods: {
        loadData(){
            this.api({url:"/api/v1/expenditures"})
            .then(res=>{
                this.expenditures = res;
            })
        },
        isReduce(expenditureType){
            var reduceValues = [20,21,22,23];
            return reduceValues.includes(expenditureType);
        }
    },
    data() {
        return {
            expenditures:[]
        }
    },
}
</script>
<style lang="css" scoped>
.expenditure-reduce,.expenditure-reduce+.expenditure-amount{
    color: #ff0000;
}

.expenditure-increment,.expenditure-increment+.expenditure-amount{
    color: #008000;
}
</style>