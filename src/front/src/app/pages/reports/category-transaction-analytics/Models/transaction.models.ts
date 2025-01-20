export interface Transaction {
    transactionDate: string;
    description: string;
    categoryDescription: string;
    totalAmount: string;
    type: string;
  }

  export interface ReportCategoryAnalytics {
    categoryDescription: string;
    transactions: Transaction[];
  }