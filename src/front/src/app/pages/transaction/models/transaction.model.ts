
export interface TransactionModel {
    transactionId: string;
    categoryBalance: number;
    type: number
    categoryId: string;
    category: string;
    description: string;
    userId: string;
    amount: number;
    transactionDate: Date
}