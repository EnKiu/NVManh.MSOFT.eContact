var Enum = {
    FormMode: {
        ADD: 1,
        UPDATE: 2,
        VIEW: 3
    },
    MsgType: {
        Success: 1,
        Info: 2,
        Error: 3,
        Confirm: 4,
        Warning: 5,
        Question: 6
    },
    Role: {
        Administrator: 1,
        Management: 5,
        Employee: 10,
        Teacher: 15,
        Advisor: 20,
        HRIntern: 21,
        Fresher: 25,
        Intern: 30,
        Newbie: 35
    },
    MessageGroupType: {
        Private: 1,
        Group: 2,
        Public: 3
    },
    KeyCode: {
        ENTER: 13
    },

    ExpenditureType: {
        INCREMENT_PLAN: 1,
        INCREMENT_SUPER_RICH: 2,
        INCREMENT_OTHER: 3,
        REDURE_PLAN: 20,
        REDURE_WEDDING: 21,
        REDURE_FUNERAL: 22,
        REDUCE_MEDICAL: 23,
        REDUCE_OTHER: 24
    },
    /**
     * Kế hoạch thu chi
     */
    ExpenditurePlanType: {
        // Thu cho sự kiện
        INCREMENT_EVENT: 100,
        // Thu quỹ hàng năm
        INCREMENT_ANNUAL: 101,
        // Thu khác
        INCREMENT_OTHER: 102,
        // Chi cho sự kiện
        REDURE_EVENT: 200,
        // Chi khác
        REDUCE_OTHER: 201
    },
    ReceiptType: {
        Income: 1,
        Outcome: 2
    },
    OptionExpenditurePlanType: {
        ForPlan: 1,
        ForOther: 2
    }
}
export default Enum;