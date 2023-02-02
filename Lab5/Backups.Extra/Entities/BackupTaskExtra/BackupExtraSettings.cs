using Backups.Extra.Models.Cleaner;
using Backups.Extra.Models.MergeInstruction;
using Backups.Tools;

namespace Backups.Extra.Entities.BackupTaskExtra;

public class BackupExtraSettings
{
    private IMergeInstruction _instruction;
    private IExtraCleanerAlgorithm _algorithm;

    public BackupExtraSettings(IMergeInstruction instruction, IExtraCleanerAlgorithm algorithm)
    {
        _instruction = instruction
                       ?? throw new BackupsException("Current instruction is invalid");
        _algorithm = algorithm
                     ?? throw new BackupsException("Current algorithm is invalid");
    }

    public IMergeInstruction MergeInstruction() => _instruction;
    public IExtraCleanerAlgorithm ExtraAlgorithm() => _algorithm;

    public void SetMergeInstruction(IMergeInstruction instruction)
    {
        _instruction = instruction ?? throw new BackupsException("Invalid instruction to set");
    }

    public void SetExtraAlgorithm(IExtraCleanerAlgorithm algorithm)
    {
        _algorithm = algorithm ?? throw new BackupsException("Invalid algorithm to set");
    }
}