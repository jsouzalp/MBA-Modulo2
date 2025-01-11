import { CategoryTypeEnum } from "src/app/pages/category/enums/category-type.enum";

export interface CategoryTransactionGraphModel {
    CategoryDescription : string;
    TotalAmount : number;
    TransactionTypeEnum : CategoryTypeEnum;  // Precisa ser refatorado isso. Tem que usar o enum de transação mas por enquanto é isso
}
