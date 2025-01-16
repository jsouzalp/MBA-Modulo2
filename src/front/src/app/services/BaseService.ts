import { HttpHeaders, HttpErrorResponse } from "@angular/common/http";
import { throwError } from "rxjs";
import { LocalStorageUtils } from "../utils/localstorage";
import { environment } from "src/environments/environment";
import { ToastrService } from "ngx-toastr";
import { MessageService } from "./message.service ";

export abstract class BaseService {
    protected UrlServiceV1: string = environment.apiUrlv1;
    public LocalStorage = new LocalStorageUtils();

    constructor(protected toastr: ToastrService, private messageService: MessageService) { }


    protected getHeaderJson() {
        return {
            headers: new HttpHeaders({
                'Content-Type': 'application/json'
            })
        };
    }

    protected getAuthHeaderJson() {
        return {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                'Accept': 'application/json, text/plain, */*',
                'Authorization': `Bearer ${this.LocalStorage.getUserToken()}`
            })
        };
    }

    protected serviceError(response: Response | any) {
        let customError: string[] = [];
        let errors: string[] = [];
        let customResponse = { error: { errors: errors } }

        if (response instanceof HttpErrorResponse) {
            if (response.statusText === "Unknown Error") {
                customError.push("Ocorreu um erro desconhecido");
                response.error.errors = customError;
            }
        }
        if (response.status === 500) {
            customError.push("Ocorreu um erro no processamento, tente novamente mais tarde ou contate o nosso suporte.");

            customResponse.error.errors = customError;
            return throwError(() => customResponse);
        }
        else if (response.status === 404) {
            customError.push("O recurso solicitado não existe. Entre em contato com o suporte.");
            customResponse.error.errors = customError;
            return throwError(() => customResponse);
        }
        else if (response.status === 403) {

            customError.push("Você não tem autorização para essa ação. Faça login novamente ou contate o nosso suporte.");

            customResponse.error.errors = customError;
            return throwError(() => customResponse);
        }
        else if (response.status === 401) {
            window.location.href = '/login';
        }

        return throwError(() => response);
    }

    protected extractData(response: any): any {
        if (response.notifications && response.notifications.length > 0) {
            response.notifications.forEach((notification: any) => {
                switch (notification.type) {
                    case 1:
                        this.toastr.success(notification.message, 'Sucesso');
                        this.messageService.setMessage('Sucesso', notification.message);
                        break;
                    case 2:
                        this.toastr.warning(notification.message, 'Atenção');
                        this.messageService.setMessage('Atenção', notification.message);
                        break;
                    case 3:
                        this.toastr.error(notification.message, 'Erro');
                        this.messageService.setMessage('Erro', notification.message);
                        break;
                    case 4:
                        this.toastr.info(notification.message, 'Informação');
                        this.messageService.setMessage('Informação', notification.message);
                        break;
                    default:
                        this.toastr.warning(notification.message, 'Notificação');
                }
            });
        }

        if (response.result) {
            return response.result || {};
        } else {
            return response;
        }
    }

    protected formatDate(date: Date): string {
        return date.toISOString().split('T')[0];
    }
}


