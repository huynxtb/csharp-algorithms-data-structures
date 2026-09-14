public class ModelConfig
{
    private string _activeModel;

    public string ActiveModel
    {
        get => _activeModel;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new System.ArgumentException("Model name cannot be empty.", nameof(value));
            _activeModel = value;
        }
    }

    public ModelConfig(string initialModel = "gemini-3.7-flash")
    {
        _activeModel = initialModel;
    }

    public void SwitchToGemini37Flash()
    {
        ActiveModel = "gemini-3.7-flash";
    }
}