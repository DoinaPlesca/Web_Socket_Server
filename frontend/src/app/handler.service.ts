import { Injectable } from '@angular/core';
import { BaseDto, ServerEchosClientDto } from '../BaseDto';

@Injectable({
  providedIn: 'root'
})
export class MessageHandlerService {

  constructor() { }

  handleIncomingMessage(message: BaseDto<any>, messages: string[]) {
    switch (message.eventType) {
      case 'ServerEchosClientDto':
        this.handleServerEchosClientDto(message as ServerEchosClientDto, messages);
        break;
      // Add cases for other message types as needed
    }
  }

  handleServerEchosClientDto(dto: ServerEchosClientDto, messages: string[]) {
    messages.push(dto.echoValue!);
  }
}
