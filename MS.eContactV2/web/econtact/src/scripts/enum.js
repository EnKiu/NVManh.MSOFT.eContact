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
    }
}
export default Enum;