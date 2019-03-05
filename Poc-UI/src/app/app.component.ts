import { Component, OnInit, OnDestroy } from '@angular/core';
import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr';
import { Apollo } from 'apollo-angular';
import { Subscription } from 'apollo-client/util/Observable';
import gql from 'graphql-tag';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit, OnDestroy {
  title = 'GraphQL subscription - SignalR PoC';
  signalr_endpoint = 'https://localhost:44337/message';
  signalrHubConnection: any;
  gqlSubscription: Subscription;

  data_from_socket: any;
  data_from_gql_subscription: any;
  constructor(private apollo: Apollo) {

  }
  ngOnInit() {

    // Establishing signalr connection
    this.signalrHubConnection = new HubConnectionBuilder()
      .withUrl(this.signalr_endpoint)
      .configureLogging(LogLevel.Information)
      .build();
    this.signalrHubConnection.start();

    // Listening to the 'BroadcastMessage' event
    this.signalrHubConnection.on('BroadcastMessage', (data: any) => {
      // Reading data from the socket
      this.data_from_socket = JSON.parse(data);
      console.log('Data from signalr : ');
      console.log(this.data_from_socket);
    });

    // Subscribing to the new_message subscription
    this.gqlSubscription = this.apollo
      .subscribe({
        query: gql`
                subscription new_message{
                  new_message{
                    id
                    message
                  }
                }`
      })
      .subscribe(result => {
        this.data_from_gql_subscription = result.data.new_message;
        console.log('Data from graphql : ');
        console.log(this.data_from_gql_subscription);
      });

  }

  ngOnDestroy() {
    this.gqlSubscription.unsubscribe();
  }
}
