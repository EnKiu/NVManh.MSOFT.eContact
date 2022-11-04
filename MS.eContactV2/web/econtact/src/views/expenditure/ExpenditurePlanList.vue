<template>
     <div class="expenditure-plan-list">
        <div class="plan">
            <div class="plan__title">Thu: Quỹ tham gia ngày hội sản xuất</div>
            <div class="plan__detail">
                <div class="fee-single">
                    <div class="expenditure__title">Phí từng thành viên:</div>
                    <div class="expenditure__fee">500.000</div>
                </div>
                <div class="fee-members-list">
                    <div class="expenditure__title">Số thành viên nộp</div>
                    <div class="expenditure__fee">15</div>
                </div>
                <div class="fee-recieved">
                    <div class="expenditure__title">Tổng đã thu:</div>
                    <div class="expenditure__fee">12.000.000</div>
                </div>
            </div>
        </div>
        <div class="plan">
            <div class="plan__title">Thu: Quỹ tham gia ngày hội sản xuất</div>
            <div class="plan__detail">
                <div class="fee-single">
                    <div class="expenditure__title">Phí từng thành viên:</div>
                    <div class="expenditure__fee">500.000</div>
                </div>
                <div class="fee-members-list">
                    <div class="expenditure__title">Số thành viên nộp</div>
                    <div class="expenditure__fee">15</div>
                </div>
                <div class="fee-recieved">
                    <div class="expenditure__title">Tổng đã thu:</div>
                    <div class="expenditure__fee">12.000.000</div>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
export default {
    name:"ExpenditurePlanList",
    emits:[],
    props:[],
    created() {
        
    },
    methods: {
        
    },
    data() {
        return {
            
        }
    },
}
</script>
<style scoped>
.plan{
    padding: 10px;
}
.plan+.plan{
    border-top: 1px solid #dedede;
}
.plan__detail{
    display: flex;
    align-items: center;
    justify-content: space-between;
}
</style>