# Clean E-Commerce Platform - Verification Script for Windows
# This script validates all deployments are working correctly

Write-Host "🔍 Verifying Clean E-Commerce Platform Deployments..." -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan

# Configuration
$ApiUrl = "https://api-caleb-ecommerce-fzbqhjhhhufzcybp.centralus-01.azurewebsites.net"
$FrontendUrl = "https://clean-ecommerce-frontend-p28nl2arx-calebjt7s-projects.vercel.app"

# Test 1: API Health Check
Write-Host ""
Write-Host "1️⃣  Testing Azure API..." -ForegroundColor Yellow

try {
    $response = Invoke-WebRequest -Uri "$ApiUrl/api/productos" -Method Get -ErrorAction Stop
    if ($response.StatusCode -eq 200) {
        Write-Host "✅ API is responding correctly (HTTP 200)" -ForegroundColor Green
    }
} catch {
    Write-Host "❌ API error: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 2: Swagger UI
Write-Host ""
Write-Host "2️⃣  Testing Swagger Documentation..." -ForegroundColor Yellow

try {
    $response = Invoke-WebRequest -Uri "$ApiUrl/swagger" -Method Get -ErrorAction Stop
    if ($response.StatusCode -eq 200) {
        Write-Host "✅ Swagger UI is available (HTTP 200)" -ForegroundColor Green
        Write-Host "   🔗 Access at: $ApiUrl/swagger" -ForegroundColor Cyan
    }
} catch {
    Write-Host "❌ Swagger error: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 3: Frontend
Write-Host ""
Write-Host "3️⃣  Testing Vercel Frontend..." -ForegroundColor Yellow

try {
    $response = Invoke-WebRequest -Uri "$FrontendUrl" -Method Get -ErrorAction Stop
    if ($response.StatusCode -eq 200) {
        Write-Host "✅ Frontend is deployed (HTTP 200)" -ForegroundColor Green
        Write-Host "   🔗 Access at: $FrontendUrl" -ForegroundColor Cyan
    }
} catch {
    Write-Host "❌ Frontend error: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 4: API Products Endpoint
Write-Host ""
Write-Host "4️⃣  Testing Products Endpoint..." -ForegroundColor Yellow

try {
    $response = Invoke-WebRequest -Uri "$ApiUrl/api/productos" -Method Get -ContentType "application/json" -ErrorAction Stop
    $products = $response.Content | ConvertFrom-Json
    Write-Host "✅ Products loaded successfully" -ForegroundColor Green
    Write-Host "   📦 Total products: $($products.Count)" -ForegroundColor Cyan
} catch {
    Write-Host "❌ Products error: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""
Write-Host "================================================" -ForegroundColor Cyan
Write-Host "✨ Verification complete!" -ForegroundColor Green
Write-Host ""
Write-Host "📝 NEXT STEPS:" -ForegroundColor Yellow
Write-Host "1. 🌐 Open in browser: $FrontendUrl" -ForegroundColor Cyan
Write-Host "2. 🔐 Log in with Admin account (bangtankpos375@gmail.com)"
Write-Host "3. 📊 Create test products at: $ApiUrl/swagger" -ForegroundColor Cyan
Write-Host "4. ✅ Check that products appear on frontend"
Write-Host "5. 🛒 Test order workflow"
