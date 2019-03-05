import { NgModule } from '@angular/core';
import { ApolloModule, APOLLO_OPTIONS } from 'apollo-angular';
import { HttpLinkModule, HttpLink } from 'apollo-angular-link-http';
import { InMemoryCache } from 'apollo-cache-inmemory';
import { WebSocketLink } from 'apollo-link-ws';
import { LinkError } from 'apollo-link/lib/linkUtils';
import { getMainDefinition } from 'apollo-utilities';
import { split } from 'apollo-link';

const luri = 'https://localhost:44337/graphql'; // <-- add the URL of the GraphQL server here
const wsuri = 'wss://localhost:44337/graphql';
const wsLink = new WebSocketLink({
  uri: wsuri,
  options: {
    reconnect: true
  }
});

export function createApollo(httpLink: HttpLink) {
  const Link = split(
    // split based on operation type
    ({ query }) => {
      const { kind, operation } = getMainDefinition(query);
      return kind === 'OperationDefinition' && operation === 'subscription';
    },
    wsLink,
    httpLink.create({
      uri: luri
    }),
  );
  return {
    link: Link,
    cache: new InMemoryCache(),
  };
}

@NgModule({
  exports: [ApolloModule, HttpLinkModule],
  providers: [
    {
      provide: APOLLO_OPTIONS,
      useFactory: createApollo,
      deps: [HttpLink],
    },
  ],
})
export class GraphQLModule {
}
