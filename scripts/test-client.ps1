

$apihost = "http://localhost:5431"

#remove backslash from the end of the URL
if ($apihost.EndsWith("/")) {
    $apihost = $apihost.Substring(0, $apihost.Length - 1)
}

$headers = @{
    "Content-Type" = "application/json"
    "Accept" = "application/json"
}


$simplecmdb = IRM -uri "$apihost/api/simplecmdb" -method GET -headers $headers


$firstSimpleCMDB = $simplecmdb[0]
$owner = $firstSimpleCMDB.owner
$project = $firstSimpleCMDB.project
$slug = $firstSimpleCMDB.slug
$webhookId = $firstSimpleCMDB.id

# Submit a test event
$newSimpleCMDBEvent = irm -uri "$apihost/api/wes/$owner/$project/$slug" -method GET -headers $headers

$newSimpleCMDBEvent = irm -uri "$apihost/api/wes/$owner/$project/$slug" -method POST -headers $headers -body (@{message = "Test message"; data = "Test data"} | ConvertTo-Json -Depth 5)


# Get an event
$webhookEvent = irm -uri "$apihost/api/webhookevents/receive/$webhookId" -method GET -headers $headers
$webhookEventId = $webhookEvent.id
# do some work on the event:

# Close the event
$res = @{
    Status = 1;
    ResultText = "SimpleCMDB event processed successfully";
} | ConvertTo-Json -Depth 5
$webhookEvent = irm -uri "$apihost/api/webhookevents/return/$webhookEventId" -method PUT -headers $headers -body $res