
Endpoints::
_UI        => /healthchecks-ui
_ActualApi => /api/health/live
_Checks    => /api/health/ready
_DataToUI  => /api/health/ui

Readme => 
ToUse
#01.SQL > change sqlconnection
	 > run migration "update-database"
#02.DummyBookApi > set multiple startup projects or run it appart, (check de ports..)