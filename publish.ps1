# Define o caminho do diretório de publicação e do arquivo de projeto
$publishPath = "C:\DEV\ProjetoLanchonete\LanchoneteApi\LanchoneteApi\publish"
$projectPath = "C:\DEV\ProjetoLanchonete\LanchoneteApi\LanchoneteApi\LanchoneteApi.csproj"

# Limpa o diretório de publicação, excluindo todos os arquivos e subdiretórios
if (Test-Path $publishPath) {
    Remove-Item -Path $publishPath -Recurse -Force
    Write-Host "Diretório de publicação limpo."
} else {
    Write-Host "Diretório de publicação não encontrado. Nada para limpar."
}

# Cria o diretório de publicação novamente para garantir que exista
New-Item -ItemType Directory -Path $publishPath

# Publica o projeto
dotnet publish $projectPath -c Release -o $publishPath

Write-Host "Projeto publicado com sucesso."