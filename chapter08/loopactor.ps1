while($true)
{
     $i++
     curl http://127.0.0.1:5014/v1.0/actors/ReservationItemActor/crazycookie/method/GetBalance
     Write-Host We have counted up to $i
}