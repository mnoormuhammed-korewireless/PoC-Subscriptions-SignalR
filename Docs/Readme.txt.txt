Before starting we need to setup cassandra db
Run these queries in the CQL shell
 - CREATE KEYSPACE poc WITH replication = {'class':'SimpleStrategy', 'replication_factor':3};
 - CREATE TABLE messages(
   ... id uuid PRIMARYKEY
   ... message text);