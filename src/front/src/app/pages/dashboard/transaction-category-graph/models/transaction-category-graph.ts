import { CategoryTypeEnum } from "src/app/pages/category/enums/category-type.enum";

export interface CategoryTransactionGraphModel {
    categoryDescription: string;
    totalAmount: number;
    quantity: number;
}
