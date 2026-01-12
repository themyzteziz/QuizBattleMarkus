
# HttpClientDemo

Konsolklient som demonstrerar async/await, timeout, cancellation och tolkning av statuskoder mot MinimalApiDemo.

## Bygga & köra
Se till att API:t körs först.
```bash
cd HttpClientDemo
 dotnet restore
 dotnet run
```
Anpassa bas-URL via miljövariabel:
```bash
# Exempel om API kör på annan port
$env:DEMO_API_BASE="http://localhost:5280"   # PowerShell
export DEMO_API_BASE=http://localhost:5280    # Bash
```

## Meny
1) GET /items – hämta alla
2) GET /items/9999 – demonstrerar 404
3) POST /items – skapa nytt (201)
4) POST /items – duplikat → 409
5) GET /items/slow?ms=5000 – visa timeout med 2s
6) GET /items/slow?ms=5000 – visa manuell cancellation (tangent)
