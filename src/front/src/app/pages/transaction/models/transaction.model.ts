
export interface TransactionModel {
    transactionId:string;
    categoryId: string;
    description: string;
    userId: string;
    amount:number;
    transactionDate:Date
}