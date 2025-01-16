import { CategoryTypeEnum } from "src/app/pages/category/enums/category-type.enum";

export interface TransactionYearEvolutionGraphModel {
    year: number;
    month: number;
    totalIncome: number;
    totalExpense: number;
    totalBalance: number;
}
