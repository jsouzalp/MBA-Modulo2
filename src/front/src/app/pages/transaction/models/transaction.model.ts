
export interface TransactionModel {
    transactionId: string;
    description: string;
    amount: number;
    type: number
    categoryId: string;
    userId: string;
    transactionDate: Date
}