import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { MaterialModule } from 'src/app/material.module';
import { MessageService } from 'src/app/services/message.service ';
import { TransactionAddComponent } from '../transaction/transaction-add.component';
import { NotificationMessage } from 'src/app/models/notificationMessage.model';


@Component({
    selector: 'app-messages',
    standalone: true,
    templateUrl: './messages.component.html',
    imports: [CommonModule, MaterialModule],
})
export class MessagesComponent implements OnInit {
    messages: NotificationMessage[] = [];
    displayedColumns: string[] = ['message', 'type','delete'];

    constructor(private dialogRef: MatDialogRef<TransactionAddComponent>,
        private messageService: MessageService) { }

    ngOnInit(): void {
        this.getMessages();
    }

    getMessages(): void {
        this.messages = this.messageService.getMessages();
    }

    deleteMessage(id: number): void {
        this.messageService.deleteMessage(id);
        this.getMessages();
    }

    close() {
        this.dialogRef.close();
    }
}