function testCommand ($command) {
    Write-Host "Testing: $command";

    $data = New-Object System.Collections.ArrayList;
    
    for ($i = 0; $i -lt 100; $i++) {
        $output = Invoke-Expression $baseCommand;
        $output[1] -match " (\d+) " | Out-Null;
        $data.Add([int]$Matches[1]) | Out-Null;
    }
    
    $data | Measure-Object -Average -Minimum -Maximum    
    Write-Host "std dev: $(get-standarddeviation($data))"
}

function get-standarddeviation {            
    [CmdletBinding()]            
    param (            
      [double[]]$numbers            
    )            
                
    $avg = $numbers | Measure-Object -Average | select Count, Average            
                
    $popdev = 0            
                
    foreach ($number in $numbers){            
      $popdev +=  [math]::pow(($number - $avg.Average), 2)            
    }            
                
    $sd = [math]::sqrt($popdev / ($avg.Count-1))            
    $sd            
}

$baseCommand = "dotnet.exe .\SudokuSolver.dll -f .\hardest.txt -ps -d 2";
testCommand($baseCommand);
