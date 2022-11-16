<!-- Thông tin Thu -->
<template>
  <div class="fluctuation">
    <div class="fluctuation__name">
      <div class="fluctuation__date">
        {{ commonJs.formatDateTime(item.ExpenditureDate) }}
      </div>
      <div class="fluctuation__title">
        {{ item.ExpenditureName }}
      </div>
      <div
        class="fluctuation__description"
        :class="isIncome ? '--color-green' : '--color-red'"
      >
        {{ item.Description }}
      </div>
    </div>
    <div class="flex">
      <div class="fluctuation__amount">
        <div
          v-if="item.Amount"
          class="expenditure__fee"
          :class="isIncome ? '--color-green' : '--color-red'"
        >
          <span v-if="isIncome">+</span><span v-else>-</span
          >{{ this.commonJs.formatMoney(item.Amount) }}
        </div>
      </div>
      <div v-if="isAdmin" class="fluctuation__option">
        <button
          class="btn-mini --color-red"
          title="Xóa khoản thu"
          @click="onDeleteFluctuation(item)"
        >
          <i class="icofont-ui-delete"></i>
        </button>
        <button
          class="btn-mini --color-edit"
          title="Sửa khoản"
          @click="onUpdateFluctuation(item)"
        >
          <i class="icofont-ui-edit"></i>
        </button>
      </div>
    </div>
  </div>
</template>
<script>
import router from "@/router";
import Enum from "@/scripts/enum";
export default {
  name: "FluctuationItem",
  emits: ["onAfterDelete"],
  props: {
    isIncome: {
      type: Boolean,
      default: false,
    },
    item: Object,
    isAdmin: Boolean,
  },
  computed: {
    type() {
      if (this.isIncome) {
        return Enum.ReceiptType.Income;
      }
      return Enum.ReceiptType.Outcome;
    },
  },
  created() {},
  methods: {
    onDeleteFluctuation(item) {
      this.commonJs.showConfirm("Bạn có chắc chắn muốn xóa khoản này không?", () => {
        this.api({
          url: `api/v1/expenditures/${item.ExpenditureId}`,
          method: "DELETE",
        }).then(() => {
          var pathName = this.isIncome ? "revenues" : "expenditures";
          this.$emit("onAfterDelete");
          router.push(`/funds?tab=${pathName}`);
        });
      });
    },
    onUpdateFluctuation(item) {
      router.push(`/funds/${item.ExpenditureId}?type=${this.type}`);
    },
  },
  data() {
    return {
      fluctuation: {},
    };
  },
};
</script>
<style scoped>
.fluctuation {
  padding: 10px;
  display: flex;
  align-items: center;
  justify-content: space-between;
}
.fluctuation + .fluctuation {
  border-top: 1px solid #dedede;
}
.fluctuation__date {
  font-weight: 700;
}
.fluctuation__detail {
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.fluctuation__description {
  font-size: 11px;
}

/* .fluctuation__name{
  margin-right: 8px;
} */

.fluctuation__amount {
  margin: 0 8px;
  font-weight: 700;
}
.btn-mini + .btn-mini {
  margin-left: 4px;
}

.fluctuation__option {
  width: 45px;
}
</style>
