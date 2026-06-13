#!/bin/bash
# Clean E-Commerce Platform - Verification Script
# This script validates all deployments are working correctly

echo "Verifying Clean E-Commerce Platform Deployments..."
echo "================================================"

# Test 1: API Health Check
echo ""
echo "Testing Azure API..."
API_URL="https://api-caleb-ecommerce-fzbqhjhhhufzcybp.centralus-01.azurewebsites.net"
API_RESPONSE=$(curl -s -o /dev/null -w "%{http_code}" "$API_URL/api/productos")

if [ "$API_RESPONSE" = "200" ]; then
    echo "API is responding correctly (HTTP $API_RESPONSE)"
else
    echo "API returned HTTP $API_RESPONSE"
fi

# Test 2: Swagger UI
echo ""
echo "Testing Swagger Documentation..."
SWAGGER_RESPONSE=$(curl -s -o /dev/null -w "%{http_code}" "$API_URL/swagger")

if [ "$SWAGGER_RESPONSE" = "200" ]; then
    echo "Swagger UI is available (HTTP $SWAGGER_RESPONSE)"
    echo "   Access at: $API_URL/swagger"
else
    echo "Swagger returned HTTP $SWAGGER_RESPONSE"
fi

# Test 3: Frontend
echo ""
echo "Testing Vercel Frontend..."
FRONTEND_URL="https://clean-ecommerce-frontend-p28nl2arx-calebjt7s-projects.vercel.app"
FRONTEND_RESPONSE=$(curl -s -o /dev/null -w "%{http_code}" "$FRONTEND_URL")

if [ "$FRONTEND_RESPONSE" = "200" ]; then
    echo "Frontend is deployed (HTTP $FRONTEND_RESPONSE)"
    echo "   Access at: $FRONTEND_URL"
else
    echo "Frontend returned HTTP $FRONTEND_RESPONSE"
fi

# Test 4: API Products Endpoint
echo ""
echo "Testing Products Endpoint..."
PRODUCTS_RESPONSE=$(curl -s "$API_URL/api/productos" | jq '. | length' 2>/dev/null || echo "Error")

if [ "$PRODUCTS_RESPONSE" != "Error" ]; then
    echo "Products loaded successfully"
    echo "   Total products: $PRODUCTS_RESPONSE"
else
    echo "Failed to load products"
fi

echo ""
echo "================================================"
echo "Verification complete!"
echo ""
echo "NEXT STEPS:"
echo "1. Log in at: $FRONTEND_URL"
echo "2. Create test products at: $API_URL/swagger"
echo "3. Check that products appear on frontend"
echo "4. Test order workflow"
